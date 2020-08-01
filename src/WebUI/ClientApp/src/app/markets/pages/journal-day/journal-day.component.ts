import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { TradesService } from '../../services/tradesService';
import { JournalEntry } from '../../models/journal';

@Component({
  selector: 'app-journal-day',
  templateUrl: './journal-day.component.html',
  styleUrls: ['./journal-day.component.css']
})
export class JournalDayComponent implements OnInit {

  constructor(private route: ActivatedRoute, private tradesService: TradesService) { 
    const year = +this.route.snapshot.paramMap.get('year');
    const month = (+this.route.snapshot.paramMap.get('month')) - 1;
    const day = +this.route.snapshot.paramMap.get('day');

    if (year && month && day) {
      this.date = new Date(year, month, day);
      console.log(`Fetching journal for ${moment(this.date).format('yyyy-MM-DD')}`);
    } else {
      console.log('Unable to construct date from route, defaulting to today.');
      this.date = new Date();
    }
  }

  date: Date;

  previous: string = `${moment(this.date).format('MMMM YYYY')}`;

  journalEntries: JournalEntry[] = [];

  ngOnInit(): void {
    this.tradesService.fetchJournalForDates(this.date, this.date)
        .subscribe(journal => {
          console.log(`Journal for ${moment(this.date).format('yyyy-MM-DD')}`, journal);
          this.journalEntries = journal
        });
  }

}
