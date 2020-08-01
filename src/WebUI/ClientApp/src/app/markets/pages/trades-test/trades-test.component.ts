import { Component, OnInit } from '@angular/core';
import { InvestmentsClient, TradeDto } from 'src/app/firewatch-api';
import { TradesService } from '../../services/tradesService';
import { TradeJournalDay } from '../../models/tradeJournal';
import * as moment from "moment";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-trades-test',
  templateUrl: './trades-test.component.html',
  styleUrls: ['./trades-test.component.css']
})
export class TradesTestComponent implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) {
    this.accountId = +this.route.snapshot.paramMap.get('accountId');

    let year = +this.route.snapshot.paramMap.get('year');
    if (year <= 0) {
      year = new Date().getFullYear();
    }
    console.log('Year', year);
    let month = this.route.snapshot.paramMap.get('month');
    const day = +this.route.snapshot.paramMap.get('day');


    // in the blocks below, we do month - 1 because JS uses 0-indexed months, but we want
    // the route to be user friendly.
    if (month === undefined || month === null) {
      // no month provided, so we'll fetch YTD
      this.from = new Date(year, 0, 1);
      this.to = new Date(year, 11, 31);
    } else if (day === null || day === undefined) {
      // no day provided, grab for the whole month
      this.from = new Date(year, +month - 1, 1);
      this.to = new Date(year, +month, 0);
    } else {
      // a specific date was provided, fetch only trades on that day
      this.from = new Date(year, +month - 1, day);
      this.to = new Date(year, +month - 1, day);
    }
    console.log('From', this.from);
    console.log('To', this.to);
  }

  accountId: number;
  year: number;
  month: number;
  day: number;

  from: Date;
  to: Date;

  ngOnInit(): void {
    this.tradesService.fetchJournal2ForDates(this.accountId, this.from, this.to)
      .subscribe(r => {
        this.journals = r; //.sort((a, b) => b.close.valueOf() - a.close.valueOf())
      });
  }

  journals: TradeJournalDay[] = [];

}
