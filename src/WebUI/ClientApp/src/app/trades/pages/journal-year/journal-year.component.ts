import { Component, OnInit } from '@angular/core';
import { CalendarEvent, CalendarView } from 'angular-calendar';
import { TradesService } from '../../services/tradesService';
import { JournalEntry } from '../../models/journal';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-journal-year',
  templateUrl: './journal-year.component.html',
  styleUrls: ['./journal-year.component.css']
})
export class JournalYearComponent implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) { }

  ngOnInit(): void {
    let year = +this.route.snapshot.paramMap.get('year');
     
    if (year === undefined || year === null || year === 0) {
      year = new Date().getFullYear();
    }
    
    this.tradesService.fetchJournalForDates(new Date(year, 0, 1), new Date(year, 11, 31))
      .subscribe(entries => { 
        console.log('Journal entries', entries);
        this.journalEntries = entries;
      });
  }

  view: CalendarView = CalendarView.Month;

  viewDate = new Date();

  events: CalendarEvent[] = [];

  journalEntries: JournalEntry[] = [];

  getJournalEntriesForMonth(month: number): JournalEntry[] {
    const months = [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec' ];
    const entries = this.journalEntries.filter(e => e.date.month() === month);
    console.log(`Fetching journal entries for ${months[month]}`, entries);
    return entries;
  }
}
