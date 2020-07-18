import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { ImportComponent } from './pages/import/import.component';
import { JournalComponent } from './journal/journal.component';

const routes: Routes = [
  {
    path: 'import',
    component: ImportComponent
  },
  {
    path: 'journal',
    component: JournalComponent
  }
];

@NgModule({
    imports: [ RouterModule.forChild(routes) ],
    exports: [ RouterModule ]
})
export class TradesRoutingModule {}