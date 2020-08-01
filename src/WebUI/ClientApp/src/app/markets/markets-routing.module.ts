import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { ImportComponent } from './pages/import/import.component';
import { JournalComponent } from './pages/journal/journal.component';
import { JournalYearComponent } from './pages/journal-year/journal-year.component';
import { JournalDayComponent } from './pages/journal-day/journal-day.component';
import { JournalMonth2Component } from './pages/journal-month2/journal-month2.component';
import { MarketsHomeComponent } from './pages/markets-home/markets-home.component';
import { TradesTestComponent } from './pages/trades-test/trades-test.component';

const routes: Routes = [
  {
    path: '',
    component: MarketsHomeComponent,
  },
      {
        path: 'import',
        component: ImportComponent
      },
      {
        path: 'journal',
        component: JournalComponent
      },
      {
        path: ':accountId/calendar/:year/:month',
        component: JournalMonth2Component
      },
      {
        path: ':accountId/calendar/:year',
        component: JournalYearComponent
      },
      {
        path: ':accountId/calendar',
        component: JournalYearComponent
      },
      {
        path: ':accountId/journal/:year/:month/:day',
        component: TradesTestComponent // TODO
      },
      {
        path: 'test/:accountId',
        component: TradesTestComponent,
      }  
];

@NgModule({
    imports: [ RouterModule.forChild(routes) ],
    exports: [ RouterModule ]
})
export class MarketsRoutingModule {}