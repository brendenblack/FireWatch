import React from 'react';
import moment from 'moment';
import cx from "classnames"
import { asCurrency } from '../common/numbers';

interface DemoPageProps {

}

interface DemoPageState {
    selectedTags: Tag[];
    trades: Trade[];
}

class Trade {
    constructor(tags: string[] = []) {

        const choices = [
            { symbol: "AMD", price: 34.33 },
            { symbol: "AAPL", price: 344.27 },
        ]

        const choice = choices[Math.floor(Math.random() * choices.length)];

        this.date = moment();
        this.symbol = choice.symbol;    
        this.averageEntry = choice.price;
        this.quantity = Math.floor(Math.random() * 100);

        

        const absoluteMovement = Math.random() * (choice.price * 0.33);
        const change = absoluteMovement * (Math.floor(Math.random()*2) == 1 ? 1 : -1); // this will add minus sign in 50% of cases

        this.averageExit = choice.price + change;
        console.log(`Price: ${choice.price} | Movement: ${absoluteMovement} | Change: ${change} | Exit ${this.averageExit}`);
        this.return = (this.averageExit - this.averageEntry) * this.quantity;

        for (let tag of tags) {
            this.tags.push(new Tag(tag));
        }
    }

    date: moment.Moment;
    symbol: string;
    tags: Tag[] = [];
    averageEntry: number;
    averageExit: number;
    quantity = 100;
    return: number;
}

class Tag {
    constructor(tag: string) {
        this.comparison = tag;
        if (tag.indexOf(':') > 0) {
            this.type = tag.substring(0, tag.indexOf(':'));
            this.value = tag.substring(tag.indexOf(':') + 1, tag.length);
        } else {
            this.value = tag;
        }
    }

    type = "other";
    value: string;

    comparison: string;
}

const trades: Trade[] = [
    new Trade([ "entry:good idea", "exit:target" ]),
    new Trade([ "entry:HOD break", "anticipated" ]),
    new Trade([ "entry:bad idea" ]),
    new Trade([ "entry:good idea", "exit:chickened out" ]),
    new Trade([ "exit:this exit made lots of money" ]),
    new Trade([ "had a cold", "long weekend" ]),
];

interface TagPillProps {
    tag: Tag;
    onClick?(tag: Tag): void;
    isSelected?: boolean;
    onDismiss?(tag: Tag): void;
}

function TagPill(props: TagPillProps) {

    const contextualClasses = cx({
        "bg-gray-200 border-gray-700": props.tag.type === 'other',
        "bg-red-300 border-red-700": props.tag.type === 'exit',
        "bg-green-300 border-green-700": props.tag.type === 'entry',
        "bg-opacity-25": !props.isSelected,
        "cursor-pointer": props.onClick
    });

    const handleClick = function() {
        console.log('Clicking on tag', props.tag);
        if (props.onClick) {
            props.onClick(props.tag);
        }
    }
    
    const handleDismiss = function() {
        if (props.onDismiss) {
            props.onDismiss(props.tag);
        }
    }

    return (
        <div className={`font-mono py-1 px-3 border rounded-full mr-2 mb-2 inline-block w-auto whitespace-no-wrap text-sm ${contextualClasses}`} onClick={handleClick}>
            {props.tag.value} {(props.onDismiss) ? <button className="m-0 p-0 align-middle" onClick={handleDismiss}><svg viewBox="0 0 20 20" fill="currentColor" className="minus-circle w-6 h-6"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM7 9a1 1 0 000 2h6a1 1 0 100-2H7z" clipRule="evenodd"></path></svg></button> : ''}
        </div>);
}

export default class DemoPage extends React.Component<DemoPageProps, DemoPageState> {
    constructor(props: DemoPageProps) {
        super(props);
        this.state = {
            selectedTags: [],
            trades: trades
        }
        
        this.allTrades = trades;

        this.handleTagClick = this.handleTagClick.bind(this);
        this.handleAddEntryClick = this.handleAddEntryClick.bind(this);
        this.handleAddTag = this.handleAddTag.bind(this);
        this.filterTrades = this.filterTrades.bind(this);
        this.clearTagFilters = this.clearTagFilters.bind(this);
        this.handleDeleteTag = this.handleDeleteTag.bind(this);
    }

    private readonly allTrades: Trade[];

    handleTagClick(tag: Tag) {
        const selectedTags = this.state.selectedTags;

        if (selectedTags.indexOf(tag) >= 0) {
            console.log('removing tag');
            const newSelectedTags = selectedTags.filter(t => t.comparison !== tag.comparison); 
            console.log(newSelectedTags);
            this.setState({
                ...this.state,
                selectedTags: newSelectedTags
            })
            // TODO: handle remove
        } else {
            selectedTags.push(tag); 
        }
        
        this.setState({
            ...this.state,
            selectedTags: selectedTags
        });

        this.filterTrades();
    }

    handleAddEntryClick(trade: Trade) {
        console.log('Adding entry to trade', trade);
        const tag = new Tag("entry:" + Math.floor(Math.random() * 1000));
        this.handleAddTag(trade, tag);
    }

    handleAddExitClick(trade: Trade) {
        const tag = new Tag("exit:" + Math.floor(Math.random() * 1000));
        this.handleAddTag(trade, tag);
        
    }

    handleAddOtherClick(trade: Trade) {
        const tag = new Tag("other:" + Math.floor(Math.random() * 1000));
        this.handleAddTag(trade, tag);
    }

    handleAddTag(trade: Trade, tag: Tag) {
        const trades = [...this.state.trades];
        const index = trades.indexOf(trade);
        const copy = trades[index];
        copy.tags.push(tag);
        trades[index] = copy;

        this.setState({
            ...this.state,
            trades: trades
        });        
    }

    filterTrades() {
        console.log('Filtering trades with tags', this.state.selectedTags);
        if (this.state.selectedTags.length > 0) {
            const trades: Trade[] = [];
            for (let trade of this.allTrades) {
                if (this.state.selectedTags.map(t => t.comparison).every(c => trade.tags.map(t => t.comparison).includes(c))) {
                    trades.push(trade);
                }
                // trade.tags.map(t => t.comparison).every(c => this.state.selectedTags.map())
                // for (let tag of trade.tags) {
                //     if (this.state.selectedTags.map(t => t.comparison).includes(tag.comparison)) {
                //         trades.push(trade);
                //         break;
                //     }
                // }
            }

            this.setState({
                ...this.state,
                trades: trades
            });
        } else {
            this.setState({
                ...this.state,
                trades: this.allTrades
            });
        }
    }

    handleDeleteTag(trade: Trade, tag: Tag) {
        console.log(`Removing tag ${tag.comparison} from trade ${trade.symbol}`);
        const trades = [...this.state.trades];
        const index = trades.indexOf(trade);
        const copy = trades[index];
        copy.tags = trade.tags.filter(t => t.comparison !== tag.comparison);
        trades[index] = copy;

        this.setState({
            ...this.state,
            trades: trades
        });        
    }

    clearTagFilters() {
        this.setState({
            ...this.state,
            selectedTags: []
        }, () => { this.filterTrades(); });
       
        
    }

    render() {
        console.log('rendering');
        const pills: JSX.Element[] = [];
        const uniqueTags = this.allTrades.flatMap(t => t.tags).filter((item, index, arr) => arr.map(a => a.comparison).indexOf(item.comparison) === index);
        for (let tag of uniqueTags) {
            pills.push(<TagPill tag={tag} isSelected={this.state.selectedTags.includes(tag)} onClick={this.handleTagClick} />)
        }

        const tradeRows: JSX.Element[] = [];
        for (let trade of this.state.trades) {

            const resultsClasses = cx({ 
                "text-red-200": trade.return < 0,
                "text-green-200": trade.return >= 0
            });

            const entries: JSX.Element[] = [];
            for (let tag of trade.tags.filter(t => t.type === 'entry')) {
                entries.push(<TagPill tag={tag} isSelected={true} onDismiss={this.handleDeleteTag.bind(this, trade)} key={`entries_${tag.comparison}`} />);
            }

            const exits: JSX.Element[] = [];
            for (let tag of trade.tags.filter(t => t.type === 'exit')) {
                exits.push(<TagPill tag={tag} isSelected={true} onDismiss={this.handleDeleteTag.bind(this, trade, tag)} key={`exits_${tag.comparison}`} />);
            }

            const others: JSX.Element[] = [];
            for (let tag of trade.tags.filter(t => t.type !== 'exit' && t.type !== 'entry')) {
                others.push(<TagPill tag={tag} isSelected={true} onDismiss={this.handleDeleteTag.bind(this, trade, tag)} key={`others_${tag.comparison}`} />);
            }

            tradeRows.push(<tr className="">
                <td className="text-gray-100 px-3 py-1">{trade.date.format('YYYY-MM-DD')}</td>
                <td className="text-gray-100 px-3 py-1">{trade.date.format('hh:mm:ss')}</td>
                <td className="text-gray-100 px-3 py-1">{trade.symbol}</td>
                <td className="text-gray-100 px-3 py-1 text-right">{trade.quantity}</td>
                <td className="text-gray-100 px-3 py-1 text-right">{asCurrency(trade.averageEntry)}</td>
                <td className="text-gray-100 px-3 py-1 text-right">{asCurrency(trade.averageExit)}</td>
                <td className={`px-3 py-1 text-right ${resultsClasses}`}>{asCurrency(trade.return)}</td>
                <td className="">{entries} <button className="text-gray-300" onClick={this.handleAddEntryClick.bind(this, trade)}><svg viewBox="0 0 20 20" fill="currentColor" className="plus-circle w-6 h-6"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-11a1 1 0 10-2 0v2H7a1 1 0 100 2h2v2a1 1 0 102 0v-2h2a1 1 0 100-2h-2V7z" clipRule="evenodd"></path></svg></button></td>
                <td className="">{exits} <button className="text-gray-300" onClick={this.handleAddExitClick.bind(this, trade)}><svg viewBox="0 0 20 20" fill="currentColor" className="plus-circle w-6 h-6"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-11a1 1 0 10-2 0v2H7a1 1 0 100 2h2v2a1 1 0 102 0v-2h2a1 1 0 100-2h-2V7z" clipRule="evenodd"></path></svg></button></td>
                <td className="">{others} <button className="text-gray-300" onClick={this.handleAddOtherClick.bind(this, trade)}><svg viewBox="0 0 20 20" fill="currentColor" className="plus-circle w-6 h-6"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-11a1 1 0 10-2 0v2H7a1 1 0 100 2h2v2a1 1 0 102 0v-2h2a1 1 0 100-2h-2V7z" clipRule="evenodd"></path></svg></button></td>
            </tr>)
        }

        return (
            <div className="flex h-screen flex-wrap bg-gray-800">
                <main className="w-3/4 p-3">
                    <table className="table-auto w-full">
                        <thead>

                        </thead>
                        <tbody>
                            {tradeRows}
                        </tbody>
                    </table>

                </main>
                <aside className="w-1/4">
                    <div className="m-3 max-h-1/4 bg-gray-100">
                        <h3 className="bg-gray-900 text-gray-100 text-lg font-sans p-1 flex flex-row">
                            <span className="flex-grow">Filters</span>
                            <button className="text-sm font-mono border px-1" onClick={this.clearTagFilters}>Clear filters</button>
                        </h3>
                        <div className="w-full flex flex-wrap p-1 bg-gray-500 border border-gray-900">
                            {pills}
                        </div>
                    </div>
                </aside>
            </div>

        );
    }
}