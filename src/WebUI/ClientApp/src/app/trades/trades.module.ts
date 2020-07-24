import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './pages/journal/journal.component';
import { ImportComponent } from './pages/import/import.component';
import { ImportTradelogComponent } from './components/import-tradelog/import-tradelog.component';
import { TradesRoutingModule } from './trades-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { JournalEntryComponent } from './components/journal-entry/journal-entry.component';
import { TradesService } from './services/tradesService';
import { MonthlyCalendarComponent } from './components/monthly-calendar/monthly-calendar.component';
import { CalendarModule } from 'angular-calendar';
import { JournalMonthComponent } from './pages/journal-month/journal-month.component';
import { JournalYearComponent } from './pages/journal-year/journal-year.component';
import { JournalDayComponent } from './pages/journal-day/journal-day.component';


@NgModule({
  declarations: [
    JournalComponent, 
    ImportComponent, 
    ImportTradelogComponent, JournalEntryComponent, JournalYearComponent, MonthlyCalendarComponent, JournalMonthComponent, JournalDayComponent
  ],
  imports: [
    CommonModule,
    TradesRoutingModule,
    ReactiveFormsModule,
    CalendarModule
  ],
  providers: [
    TradesService
  ]
})
export class TradesModule { }
