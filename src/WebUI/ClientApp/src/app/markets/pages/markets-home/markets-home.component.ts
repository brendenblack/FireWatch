import { Component, OnInit } from '@angular/core';
import { AccountsClient, AccountDto } from 'src/app/firewatch-api';

@Component({
  selector: 'app-markets-home',
  templateUrl: './markets-home.component.html',
  styleUrls: ['./markets-home.component.css']
})
export class MarketsHomeComponent implements OnInit {

  constructor(private accountsClient: AccountsClient) { }

  ngOnInit(): void {
    this.accountsClient.getAccounts().subscribe(vm => {
      console.log(vm);
      this.accounts = vm.accounts;
    });
  }

  accounts: AccountDto[] = [];

}
