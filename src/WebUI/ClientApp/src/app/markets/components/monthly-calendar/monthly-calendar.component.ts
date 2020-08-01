import { Component, OnInit, Input, OnChanges } from '@angular/core';
import * as moment from 'moment';
import { TradeJournalDay } from '../../models/tradeJournal';

@Component({
  selector: 'app-monthly-calendar',
  templateUrl: './monthly-calendar.component.html',
  styleUrls: ['./monthly-calendar.component.css']
})
export class MonthlyCalendarComponent implements OnChanges {

  constructor() { }

 

  ngOnChanges() {
    const date = new Date(this.year, this.month);
    const firstDay = (date).getDay();
    const daysInMonth = this.daysInMonth(this.year, this.month);
    // this.date = moment(new Date(this.year, this.month, 1));
    this.monthName = moment(date).format('MMMM');

    let dateCounter = 1;
    const calendarRows: CalendarRow[] = [];
    for (let i = 0; i < 6; i++) {
      let row = new CalendarRow();
      for (let j = 0; j < 7; j++) {
        if (i === 0 && j < firstDay) {
          // calendar spots before this month begins
          row.cells.push(new CalendarCell(null, null));
        } else if (dateCounter > daysInMonth) {
          // month over
          break;
        } else {
          const cellDate = new Date(this.year, this.month, dateCounter);
          const journalEntriesForDate = this.journalEntries.filter(j => j.date.isSame(moment(new Date(this.year, this.month, dateCounter))));
          const journalEntry = (journalEntriesForDate.length > 0) ? journalEntriesForDate[0] : null;
          const cell = new CalendarCell(cellDate, journalEntry);
          row.cells.push(cell);
          dateCounter++;
        }
      }
      calendarRows.push(row);
    }

    this.calendarRows = calendarRows;
  }

  calendarRows: CalendarRow[] = [];

  daysInMonth(year: number, month: number): number {
    return 32 - new Date(year, month, 32).getDate();
  }

  @Input() accountId: number;

  @Input() month: number;

  @Input() year: number;

  @Input() journalEntries: TradeJournalDay[] = [];

  @Input() small: boolean = false;

  monthName: string;
}

class CalendarRow {
  cells: CalendarCell[] = [];
}


class CalendarCell {
  constructor(date: Date | null, journal: TradeJournalDay | null) {
    this.date = date;
    this.journal = journal;

    this.displayDate = (this.date === null) ? '' : this.date.getDate().toString();

    this.classesToApply.push('cell');
    this.classesToApply.push('day');

    if (this.date === null) {
      this.classesToApply.push('invalid');
    } else if (this.journal === null || this.journal.pnl === 0) {
      this.classesToApply.push('draw');
    } else if (this.isWeekend) {
      this.classesToApply.push('weekend');
    } else if (this.journal.pnl > 0) {
      this.classesToApply.push('win');
    } else if (this.journal.pnl < 0) {
      this.classesToApply.push('loss');
    }

    
  }

  displayDate: string;

  private classesToApply: string[] = [];

  get classes(): string {
    return this.classesToApply.join(' ');
  }

  date: Date | null;

  journal: TradeJournalDay | null;

  isWeekend: boolean = (this.date && (this.date.getDay() === 0 || this.date.getDay() === 6));  
}

// class CalendarCell {
//   constructor(date: Date | null, journal: JournalEntry | null) {
//     this.date = date;
//     this.journal = journal;

//     this.displayDate = (this.date === null) ? '' : this.date.getDate().toString();

//     this.classesToApply.push('cell');
//     this.classesToApply.push('day');

//     if (this.date === null) {
//       this.classesToApply.push('invalid');
//     } else if (this.journal === null || this.journal.totalProfitAndLoss(true) === 0) {
//       this.classesToApply.push('draw');
//     } else if (this.isWeekend) {
//       this.classesToApply.push('weekend');
//     } else if (this.journal.totalProfitAndLoss(true) > 0) {
//       this.classesToApply.push('win');
//     } else if (this.journal.totalProfitAndLoss(true) < 0) {
//       this.classesToApply.push('loss');
//     }

    
//   }

//   displayDate: string;

//   private classesToApply: string[] = [];

//   get classes(): string {
//     return this.classesToApply.join(' ');
//   }

//   date: Date | null;

//   journal: JournalEntry | null;

//   isWeekend: boolean = (this.date && (this.date.getDay() === 0 || this.date.getDay() === 6));  
// }
