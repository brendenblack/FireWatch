import { Component, OnInit } from '@angular/core';
import { ParseAndImportTradesResponse, InvestmentsClient, TradeExecutionDto, TradeDto } from 'src/app/firewatch-api';
import { ModuleMapNgFactoryLoader } from '@nguniversal/module-map-ngfactory-loader';
import * as moment from 'moment';
import { reduce } from 'rxjs/operators';
import { JournalEntry, JournalSymbol } from '../../models/journal';
import { TradesService } from '../../services/tradesService';
import { FormBuilder, FormGroup } from '@angular/forms';


interface Filter {
  display: string;
  value: string;
}

@Component({
  selector: 'app-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.css']
})
export class JournalComponent implements OnInit {

  public radioGroupForm: FormGroup;
  
  constructor(private client: InvestmentsClient, private formBuilder: FormBuilder) { 
  }

  allTrades: TradeDto[] = [];

  trades: TradeDto[] = [];

  winners: TradeDto[] = [];

  losers: TradeDto[] = [];

  vehicleFilter: 'STOCK' | 'OPTION' | undefined;

  vehicleFilters: Filter[] = [ 
    { display: 'Stock', value: 'STOCK' }, 
    { display: 'Option', value: 'OPTION' },
    { display: 'None', value: undefined }
  ];

   ngOnInit(): void {

      this.loadTrades();
  }

  daysToRetrieve = 30;

  loadTrades() {

    const from = moment(new Date()).subtract(this.daysToRetrieve, 'day');
    const to = moment(new Date());

    this.client.getTradesForAccount(1, from.format('YYYYMMDD'), to.format('YYYYMMDD'))
      .subscribe(vm => {
        console.log('Retrieved trades', vm.trades);
        this.allTrades = vm.trades;
        this.trades = vm.trades.sort((a, b) => b.close.valueOf() - a.close.valueOf());
        this.losers = vm.trades.filter(t => t.netProfitAndLoss < 0);
        this.winners = vm.trades.filter(t => t.netProfitAndLoss > 0);
      });
  }
}
