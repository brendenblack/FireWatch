import { Injectable } from "@angular/core";
import { InvestmentsClient, TradeExecutionsVm } from "src/app/firewatch-api";
import * as moment from "moment";
import { JournalEntry, JournalSymbol } from "../models/journal";
import { Observable } from "rxjs";
import { map as observableMap } from "rxjs/internal/operators/map";

@Injectable()
export class TradesService {
    constructor(private investmentsClient: InvestmentsClient) { }

    fetchJournalForDates(from: Date, to: Date): Observable<JournalEntry[]> {        
        const fromAsString = moment(from).format("yyyyMMDD");
        const toAsString = moment(to).format('yyyyMMDD');
        
        console.log(`Retrieving executions between ${fromAsString} and ${toAsString}`);
        return this.investmentsClient.getExecutions(fromAsString, toAsString)
            .pipe(observableMap((response: TradeExecutionsVm) => {
                const entries: JournalEntry[] = [];
                    console.log('Returned executions', response);

                    // TODO: is there a less shit way to do this?
                    const dates = Array.from(new Set(response.executions.map(e => moment(e.date).format('YYYY-MM-DD'))))
                      .map(d => moment(d))
                      .sort((a, b) => { return b.valueOf() - a.valueOf() }); 
            
                    for (let date of dates) {           
                      const entry = new JournalEntry(date);
            
                      const executionsOnDate = response.executions.filter(e => moment(e.date).isSame(date, 'day'));

                      for (let symbol of new Set(executionsOnDate.map(e => e.symbol))) {
                        
                        const executions = executionsOnDate.filter(e => e.symbol === symbol);

                        const journalSymbol = new JournalSymbol(symbol, executions);
                        entry.symbols.push(journalSymbol);
                      }

                      entries.push(entry);
                    }

                    return entries;
            }));
      }
}
