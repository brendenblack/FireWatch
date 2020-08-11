import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './pages/journal/journal.component';
import { ImportComponent } from './pages/import/import.component';
import { ImportTradelogComponent } from './components/import-tradelog/import-tradelog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { JournalEntryComponent } from './components/journal-entry/journal-entry.component';
import { TradesService } from './services/tradesService';
import { MonthlyCalendarComponent } from './components/monthly-calendar/monthly-calendar.component';
import { JournalMonthComponent } from './pages/journal-month/journal-month.component';
import { JournalYearComponent } from './pages/journal-year/journal-year.component';
import { JournalDayComponent } from './pages/journal-day/journal-day.component';
import { TradesTestComponent } from './pages/trades-test/trades-test.component';
import { JournalMonth2Component } from './pages/journal-month2/journal-month2.component';
import { MarketsRoutingModule } from './markets-routing.module';
import { MarketsHomeComponent } from './pages/markets-home/markets-home.component';
import { RouterModule } from '@angular/router';
import { AverageReturnComponent } from './components/average-return/average-return.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    JournalComponent, 
    ImportComponent, 
    ImportTradelogComponent, 
    JournalEntryComponent, 
    JournalYearComponent, 
    MonthlyCalendarComponent, 
    JournalMonthComponent, 
    JournalDayComponent, 
    TradesTestComponent, 
    JournalMonth2Component, 
    MarketsHomeComponent, AverageReturnComponent
  ],
  imports: [
    CommonModule,
    MarketsRoutingModule,
    ReactiveFormsModule,
    ChartsModule
  ],
  providers: [
    TradesService
  ],
})
export class MarketsModule {}
