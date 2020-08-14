import { Component, OnInit } from '@angular/core';
import { ParseAndImportTradesResponse, InvestmentsClient, TradeExecutionDto, TradeDto } from 'src/app/firewatch-api';
import { ModuleMapNgFactoryLoader } from '@nguniversal/module-map-ngfactory-loader';
import * as moment from 'moment';
import { reduce, filter } from 'rxjs/operators';
import { JournalEntry, JournalSymbol } from '../../models/journal';
import { TradesService } from '../../services/tradesService';
import { FormBuilder, FormGroup } from '@angular/forms';


interface Filter<T> {
  display: string;
  value: T;
}

type VehicleFilter = 'STOCK' | 'OPTION' | undefined;
type SideFilter = 'LONG' | 'SHORT' | undefined;

@Component({
  selector: 'app-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.css']
})
export class JournalComponent implements OnInit {

  public radioGroupForm: FormGroup;
  
  constructor(private client: InvestmentsClient, private formBuilder: FormBuilder) { 
  }

  /**
   * @description Holds the entire collection of trades returned from the service.
   */
  private allTrades: TradeDto[] = [];

  /**
   * @description The trades that match the current filters and should be displayed.
   */
  trades: TradeDto[] = [];

  /**
   * @description All winning trades that match the current filter. A winning trade is
   * a trade that is closed and has positive net proceeds. 
   */
  winners: TradeDto[] = [];

  /**
   * @description All losing trades that match the current filter. A losing trade is
   * a trade that is closed and has negative net proceeds.
   */
  losers: TradeDto[] = [];

  vehicleFilter: VehicleFilter = undefined;
  sideFilter: SideFilter = undefined;

  vehicleFilters: Filter<VehicleFilter>[] = [ 
    { display: 'Stock', value: 'STOCK' }, 
    { display: 'Option', value: 'OPTION' },
    { display: 'None', value: undefined }
  ];

  sideFilters: Filter<SideFilter>[] = [
    { display: 'None', value: undefined },
    { display: 'Long', value: 'LONG' },
    { display: 'Short', value: 'SHORT' },
  ]

   ngOnInit(): void {

      this.loadTrades();
  }

  daysToRetrieve = 90;

  loadTrades() {

    const from = moment(new Date()).subtract(this.daysToRetrieve, 'day');
    const to = moment(new Date());

    this.client.getTradesForAccount(1, from.format('YYYYMMDD'), to.format('YYYYMMDD'))
      .subscribe(vm => {
        console.log('Retrieved trades', vm.trades);
        this.allTrades = vm.trades;
        this.trades = this.applyFilters(this.allTrades).sort((a, b) => b.close.valueOf() - a.close.valueOf());
        this.losers = this.trades.filter(t => t.isClosed && t.netProfitAndLoss < 0);
        this.winners = this.trades.filter(t => t.isClosed && t.netProfitAndLoss > 0);
      });
  }

  applyFilters(trades: TradeDto[]): TradeDto[] {
    let filteredTrades: TradeDto[] = trades;
    
    if (this.vehicleFilter) {
      filteredTrades = filteredTrades.filter(t => t.vehicle === this.vehicleFilter);
    }

    if (this.sideFilter) {
      filteredTrades = filteredTrades.filter(t => t.side === this.sideFilter);
    }

    // TODO: support other filters

    return filteredTrades;
  }
}
