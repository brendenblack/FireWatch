import { TradeDto } from "src/app/firewatch-api";
import { JournalExecution } from "./journal";


/**
 * 
 */
export class TradeJournalDay {
    constructor(date: moment.Moment, dtos: TradeDto[]) {
        this.date = date;
        this.trades = dtos.filter(dto => dto.isClosed && date.isSame(dto.close, 'day'))
            .map(dto => new JournalTrade(dto));

            
        this.pnl = this.trades.map(t => t.netProceeds).reduce((a, b) => a + b);
    }

    date: moment.Moment;

    trades: JournalTrade[] = [];

    pnl: number;
}

export class JournalTrade {
    constructor(dto: TradeDto) {
        this.symbol = dto.symbol;
        this.vehicle = dto.vehicle;

        this.open = dto.open;
        this.close = dto.close;

        this.executions = dto.executions.map(e => new JournalExecution(e));

        this.side = dto.side;
        this.state = dto.state;
        this.isClosed = dto.isClosed;
        this.isIntraday = dto.isIntraDay;
        this.volume = dto.volume;
        this.grossProceeds = dto.grossProfitAndLoss;
        this.netProceeds = dto.netProfitAndLoss;

        let entryNumerator = 0;
        let entryDenominator = 0;
        let closingNumerator = 0;
        let closingDenominator = 0;
        for (let execution of this.executions) {
            if (execution.intent === 'OPENING') {
                entryNumerator += execution.unitPrice.amount * execution.quantity;
                entryDenominator += execution.quantity;
            } else if (execution.intent === 'CLOSING') {
                closingNumerator += execution.unitPrice.amount * execution.quantity;
                closingDenominator += execution.quantity;
            }
        }

        this.averageEntry = entryNumerator / entryDenominator;
        this.averageExit = closingNumerator / closingDenominator;

        if (this.vehicle === 'OPTION') {
            this.percentReturn = this.netProceeds / (this.averageEntry * 100 * this.volume);
        } else if (this.vehicle === 'STOCK') {
            this.percentReturn = this.netProceeds / (this.averageEntry * this.volume);
        }       
    }

    symbol: string;
    vehicle: string;

    open: moment.Moment;
    close: moment.Moment;

    grossProceeds: number;
    netProceeds: number;
    percentReturn: number;

    state: string;
    side: string;
    isClosed: boolean;
    isIntraday: boolean;
    volume: number;

    executions: JournalExecution[] = [];

    averageEntry: number;
    averageExit: number;
}