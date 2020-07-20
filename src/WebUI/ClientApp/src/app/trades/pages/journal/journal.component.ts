import { Component, OnInit } from '@angular/core';
import { ParseAndImportTradesResponse, InvestmentsClient, TradeExecutionDto } from 'src/app/firewatch-api';
import { ModuleMapNgFactoryLoader } from '@nguniversal/module-map-ngfactory-loader';
import * as moment from 'moment';
import { reduce } from 'rxjs/operators';
import { JournalEntry, JournalSymbol } from '../../models/journal';
import { TradesService } from '../../services/tradesService';

class JournalViewModel {
  entries: JournalEntry[] = [];
}



@Component({
  selector: 'app-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.css']
})
export class JournalComponent implements OnInit {

  debug = true;

  vm = new JournalViewModel();
  
  constructor(private investmentsClient: InvestmentsClient, private tradesService: TradesService) { 
  }

  ngOnInit(): void {
      this.tradesService.fetchJournalForDates(new Date(new Date().setDate(new Date().getDate() - 7 )), new Date())
        .subscribe(journal => console.log(journal));
  }

  // fetchExecutionsBetween(from: Date, to: Date) {
    
  //   const fromAsString = moment(from).format("yyyyMMDD");
  //   const toAsString = moment(to).format('yyyyMMDD');
    
  //   console.log(`Retrieving executions between ${fromAsString} and ${toAsString}`);
  //   this.investmentsClient.getExecutions(fromAsString, toAsString)
  //     .subscribe(result => {
  //       console.log('Returned executions', result);
        
  //       // clear existing data
  //       const vm = new JournalViewModel();

  //       // TODO: is there a less shit way to do this?
  //       const dates = Array.from(new Set(result.executions.map(e => moment(e.date).format('YYYY-MM-DD'))))
  //         .map(d => moment(d))
  //         .sort((a, b) => { return b.valueOf() - a.valueOf() }); 

  //       for (let date of dates) {
  //         const dateKey = date.format('yyyy-MM-DD');

  //         const entry = new JournalEntry(date);

  //         const executionsOnDate = result.executions.filter(e => moment(e.date).isSame(date, 'day'));

  //         for (let symbol of new Set(executionsOnDate.map(e => e.symbol))) {
  //           const journalSymbol = new JournalSymbol(symbol);
  //           const executions = executionsOnDate.filter(e => e.symbol === symbol);
  //           journalSymbol.executions = executions;
  //           entry.symbols.push(journalSymbol);
  //         }
  //         vm.entries.push(entry);
  //       }

  //       this.vm = vm;

  //       console.log('View model', this.vm);
  //     });
  // }
}
