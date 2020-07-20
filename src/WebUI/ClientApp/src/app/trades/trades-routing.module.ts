import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { ImportComponent } from './pages/import/import.component';
import { JournalComponent } from './pages/journal/journal.component';
import { JournalMonthComponent } from './pages/journal-month/journal-month.component';
import { JournalYearComponent } from './pages/journal-year/journal-year.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'calendar',
    pathMatch: 'full',
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
    path: 'calendar/:year/:month',
    component: JournalMonthComponent
  },
  {
    path: 'calendar/:year',
    component: JournalYearComponent
  },
  {
    path: 'calendar',
    component: JournalYearComponent
  },

];

@NgModule({
    imports: [ RouterModule.forChild(routes) ],
    exports: [ RouterModule ]
})
export class TradesRoutingModule {}