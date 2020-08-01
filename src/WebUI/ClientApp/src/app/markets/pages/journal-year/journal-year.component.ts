import { Component, OnInit } from '@angular/core';
import { CalendarEvent, CalendarView } from 'angular-calendar';
import { TradesService } from '../../services/tradesService';
import { JournalEntry } from '../../models/journal';
import { ActivatedRoute } from '@angular/router';
import { TradeJournalDay } from '../../models/tradeJournal';

@Component({
  selector: 'app-journal-year',
  templateUrl: './journal-year.component.html',
  styleUrls: ['./journal-year.component.css']
})
export class JournalYearComponent implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) { }

  ngOnInit(): void {
    console.log('init');
    this.accountId = +this.route.snapshot.paramMap.get('accountId');
    this.year = +this.route.snapshot.paramMap.get('year');
     
    if (this.year === undefined || this.year === null || this.year === 0) {
      this.year = new Date().getFullYear();
    }
    
    this.tradesService.fetchJournal2ForDates(this.accountId, new Date(this.year, 0, 1), new Date(this.year, 11, 31))
      .subscribe(entries => { 
        // console.log('Journal entries', entries);
        this.journalEntries = entries;
        this.monthlyPnl(5);
      });
  }

  year: number;
  accountId: number;

  view: CalendarView = CalendarView.Month;

  viewDate = new Date();

  events: CalendarEvent[] = [];

  journalEntries: TradeJournalDay[] = [];


  get ytdPnl(): number {
    return (this.journalEntries.length > 0)
      ? this.journalEntries.map(j => j.pnl).reduce((a, b) => a + b)
      : 0;
  }

  getJournalEntriesForMonth(month: number): TradeJournalDay[] {
    const months = [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec' ];
    const entries = this.journalEntries.filter(e => e.date.month() === month);
    console.log(`Fetching journal entries for ${months[month]}`, entries);
    return entries;
  }

  monthlyPnl(month: number): number {
    if (this.journalEntries.length === 0) {
      return 0;
    }

    const monthlyEntries = this.journalEntries.filter(e => e.date.month() === month);

    let pnl = 0;
    for (let entry of monthlyEntries) {
      console.log(`${entry.date.format('DD MMM YYYY')}: ${entry.pnl}`);
      if (entry.pnl > 200 || entry.pnl < -200) {
        console.log(entry);
      }
      pnl += entry.pnl;
    }

    console.log(pnl);
    
    
    return (monthlyEntries.length === 0) ? 0 : monthlyEntries
      .map(j => j.pnl)
      .reduce((a, b) => a + b);
  }
}
