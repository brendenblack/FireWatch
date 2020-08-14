import { Component, Input, OnChanges } from '@angular/core';
import { TradeDto } from 'src/app/firewatch-api';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import * as moment from 'moment';

@Component({
  selector: 'app-running-return',
  templateUrl: './running-return.component.html',
  styleUrls: ['./running-return.component.css']
})
export class RunningReturnComponent implements OnChanges {

  @Input() trades: TradeDto[];
  
  constructor() { }

  public lineChartData: ChartDataSets[] = [
    { data: [65, 59, 80, 81, 56, 55, 40] },
  ];
  public lineChartLabels: Label[] = [];
  public lineChartOptions: ChartOptions = {
    responsive: true,
    showLines: true,
    spanGaps: true,
    scales: {
      yAxes: [{
        gridLines: { display: true },
        ticks: { display: true },
        position: 'right',
      }],
      xAxes: [
        {
          gridLines: { display: false },
          type: 'time',
          distribution: 'series',
          time: {
            unit: 'week',
            minUnit: 'week',
          },
          bounds: 'data',
          ticks: {
            source: 'data',
            autoSkip: false,
            maxRotation: 90,
            display: false,
          },
        }
      ]
    }
  };
  public lineChartColors: Color[] = [
    {
      
    },
  ];
  public lineChartLegend = false;
  public lineChartType = 'line';
  public lineChartPlugins = [ { filler: { propagate: false } } ];

  ngOnChanges(): void {
    let runningTotal = 0;
    let currentDate: moment.Moment;
    const dataPoints = [];
    const sortedTrades = this.trades.sort((a, b) => a.close.valueOf() - b.close.valueOf());
    for (let i = 0; i < sortedTrades.length; i++) {
      const trade = sortedTrades[i];

      if (!trade.isClosed) {
        continue;
      }

      if (!currentDate) {
        currentDate = trade.close.startOf('day');
      } 

      if (i === sortedTrades.length - 1 || trade.close.isAfter(currentDate, 'day')) {
        // close off the day
        dataPoints.push({ t: currentDate, y: runningTotal });
        currentDate = trade.close.startOf('day');
      } 

      runningTotal += trade.netProfitAndLoss;
    }

    console.log(dataPoints);

    this.lineChartData = [{ 
      data: dataPoints, 
      label: 'Running returns',
      // pointRadius: 1,
      // pointBorderWidth: 1,
      fill: true,
      // it looks like fill as an object is a future plan, https://www.chartjs.org/docs/master/charts/area#filling-modes
      // fill: {
      //   target: 'origin',
      //   above: 'rgb(0, 0, 255)',
      //   below: 'rgb(249, 110, 143)',
      // },      
      showLine: true,
      lineTension: 0,
    }];
  }

  

}
