import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { TradesService } from '../../services/tradesService';
import { JournalEntry } from '../../models/journal';

@Component({
  selector: 'app-journal-month',
  templateUrl: './journal-month.component.html',
  styleUrls: ['./journal-month.component.css']
})
export class JournalMonthComponent implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) {
    this.year = +this.route.snapshot.paramMap.get('year');
    this.month = (+this.route.snapshot.paramMap.get('month')) - 1;
  }

  ngOnInit(): void {
    const firstDay = new Date(this.year, this.month, 1);
    const lastDay = new Date(this.year, this.month + 1, 0);

    this.tradesService.fetchJournalForDates(firstDay, lastDay)
    .subscribe(entries => { 
      console.log('Journal entries', entries);
      this.journalEntries = entries;
    });

  }

  year: number;
  month: number;


  journalEntries: JournalEntry[] = [];

  get mtdPnl(): number {
    return (this.journalEntries.length > 0 ) ? this.journalEntries
      .map(e => e.totalProfitAndLoss(true))
      .reduce((a, b) => a + b) : 0;
  }
}
