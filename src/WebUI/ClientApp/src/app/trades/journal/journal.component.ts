import { Component, OnInit } from '@angular/core';
import { ParseAndImportTradesResponse, InvestmentsClient, TradeExecutionDto } from 'src/app/firewatch-api';
import { ModuleMapNgFactoryLoader } from '@nguniversal/module-map-ngfactory-loader';
import * as moment from 'moment';
import { reduce } from 'rxjs/operators';

class JournalViewModel {
  dates: JournalDate[] = [];
}

class JournalDate {
  constructor(date: moment.Moment) {
    this.date = date;
  }

  date: moment.Moment;
  symbols: JournalSymbol[] = [];

  totalTrades(): number {
    return this.symbols
      .map(s => s.executions.length)
      .reduce((a, b) => a + b, 0);
  }

  totalVolume(): number {
    let volume = 0;
    
    for (let symbol of this. symbols) {
      for (let execution of symbol.executions) {
        volume += Math.abs(execution.quantity);
      }
    }

    return volume;
  }

  totalFees(): number {
    let fees = 0;
    
    for (let symbol of this. symbols) {
      for (let execution of symbol.executions) {
        fees += Math.abs(execution.fees.amount);
      }
    }

    return fees;
  }

  totalCommissions(): number {
    let commissions = 0;
    
    for (let symbol of this. symbols) {
      for (let execution of symbol.executions) {
        commissions += Math.abs(execution.commissions.amount);
      }
    }

    return commissions;
  }
}

class JournalSymbol {
  constructor(symbol: string) {
    this.symbol = symbol;
  }
  symbol: string;
  executions: TradeExecutionDto[] = [];

  executionCount() {
    return this.executions.length;
  }

  volume(): number {
    let volume = 0;

    for (let execution of this.executions) {
      volume += Math.abs(execution.quantity);
    }

    return volume;
  }

  profitAndLoss(includeFeesAndCommissions: boolean = false): number {
    let pnl = 0;
    for (let execution of this.executions) {
      pnl += execution.unitPrice.amount * execution.quantity * - 1
      if (includeFeesAndCommissions) {
        pnl -= execution.fees.amount;
        pnl -= execution.commissions.amount;
      }
    }
    return pnl;
  }
}

@Component({
  selector: 'app-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.css']
})
export class JournalComponent implements OnInit {

  debug = true;

  vm = new JournalViewModel();
  
  constructor(private investmentsClient: InvestmentsClient) { 
  }

  ngOnInit(): void {
    this.fetchExecutionsBetween(
      new Date(new Date().setDate(new Date().getDate() - 7 )), 
      new Date());    
  }

  fetchExecutionsBetween(from: Date, to: Date) {
    
    const fromAsString = moment(from).format("yyyyMMDD");
    const toAsString = moment(to).format('yyyyMMDD');
    
    console.log(`Retrieving executions between ${fromAsString} and ${toAsString}`);
    this.investmentsClient.getExecutions(fromAsString, toAsString)
      .subscribe(result => {
        console.log('Returned executions', result);
        
        // clear existing data
        const vm = new JournalViewModel();

        // TODO: is there a less shit way to do this?
        const dates = Array.from(new Set(result.executions.map(e => moment(e.date).format('YYYY/MM/DD'))))
          .map(d => moment(d))
          .sort((a, b) => { return b.valueOf() - a.valueOf() }); 

        console.log('Constructing vm for dates', dates);
        for (let date of dates) {
          const dateKey = date.format('yyyy-MM-DD');
          this.vm[dateKey] = {}
          const journalDate = new JournalDate(date);

          const executionsOnDate = result.executions.filter(e => moment(e.date).isSame(date, 'day'));

          for (let symbol of new Set(executionsOnDate.map(e => e.symbol))) {
            const journalSymbol = new JournalSymbol(symbol);
            const executions = executionsOnDate.filter(e => e.symbol === symbol);
            this.vm[dateKey][symbol] = executions;
            journalSymbol.executions = executions;
            journalDate.symbols.push(journalSymbol);
          }
          vm.dates.push(journalDate);
        }

        this.vm = vm;

        console.log('View model', this.vm);
      });
  }

}