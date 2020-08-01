import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TradesService } from '../../services/tradesService';
import { TradeJournalDay } from '../../models/tradeJournal';

@Component({
  selector: 'app-journal-month2',
  templateUrl: './journal-month2.component.html',
  styleUrls: ['./journal-month2.component.css']
})
export class JournalMonth2Component implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) {
    this.accountId = +this.route.snapshot.paramMap.get('accountId');
    this.year = +this.route.snapshot.paramMap.get('year') ?? new Date().getFullYear();
    this.month = (+this.route.snapshot.paramMap.get('month')) - 1 ?? new Date().getMonth();
  }

  accountId: number;
  year: number;
  month: number;

  journals: TradeJournalDay[] = [];


  ngOnInit(): void {
    const firstDay = new Date(this.year, this.month, 1);
    const lastDay = new Date(this.year, this.month + 1, 0);

    this.tradesService.fetchJournal2ForDates(1, firstDay, lastDay)
      .subscribe((resp: TradeJournalDay[]) => {
        this.journals = resp;
      }); 
  }

  get mtdPnl(): number {
    return (this.journals.length > 0 ) ? this.journals
      .map(e => e.pnl)
      .reduce((a, b) => a + b) : 0;
  }

}
