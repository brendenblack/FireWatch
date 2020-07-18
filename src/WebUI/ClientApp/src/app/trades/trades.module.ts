import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './journal/journal.component';
import { ImportComponent } from './pages/import/import.component';
import { ImportTradelogComponent } from './components/import-tradelog/import-tradelog.component';
import { TradesRoutingModule } from './trades-routing.module';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    JournalComponent, 
    ImportComponent, 
    ImportTradelogComponent
  ],
  imports: [
    CommonModule,
    TradesRoutingModule,
    ReactiveFormsModule,
  ]
})
export class TradesModule { }
