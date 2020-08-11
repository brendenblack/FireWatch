import { Component, Input, OnChanges } from '@angular/core';
import { TradeDto } from 'src/app/firewatch-api';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import * as moment from 'moment';

@Component({
  selector: 'app-average-return',
  templateUrl: './average-return.component.html',
  styleUrls: ['./average-return.component.css']
})
export class AverageReturnComponent implements OnChanges {

  constructor() {
    this.id = Math.random().toString(36).substring(2);
  }

  id: string;

  public areaChartData: ChartDataSets[] = [
    { data: [65, 59, 80, 81, 56, 55, 40] },
  ];
  public areaChartLabels: Label[] = [];
  public areaChartOptions: ChartOptions = {
    responsive: true,
    showLines: false,
    spanGaps: true,
    scales: {
      yAxes: [{
        gridLines: { display: false },
        ticks: { display: false }
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
            maxRotation: 0,
            display: false,
          },
        }
      ]
    }
  };
  public areaChartColors: Color[] = [
    {
      
    },
  ];
  public areaChartLegend = false;
  public areaChartType = 'line';
  public areaChartPlugins = [ { filler: { propagate: false } } ];


  ngOnChanges(): void {
    // console.log(`Initializing component for ${this.title}`, this.trades);
    this.averageReturn = (this.trades.length > 0) ? this.trades.filter(t => t.isClosed).map(t => t.netProfitAndLoss).reduce((a, b) => a+b, 0) / this.trades.filter(t => t.isClosed).length : 0;
    
    
    const tradesByDate = new Map();
    this.trades.forEach(trade => {
      const key = trade.close.format('YYYY-MM-DD');
      const collection = tradesByDate.get(key);
      if (!collection) {
        tradesByDate.set(key, [ trade ]);
      } else {
        collection.push(trade);
      }
    });

    const dataPoints = [];
    tradesByDate.forEach((v: TradeDto[], k: string) => {
      const closedTrades = v.filter(t => t.isClosed);
      if (closedTrades.length === 0) {
        return;
      }

      const avg = closedTrades.map(t => t.netProfitAndLoss).reduce((a,b) => a + b) / closedTrades.length;
      dataPoints.push({ t: moment(k), y: avg });
    });

    // for (let trade of this.trades.sort((a,b) => a.close.valueOf() - b.close.valueOf())) {
    //   dataPoints.push({ x: trade.close.toDate(), y: trade.netProfitAndLoss });
    //   labels.add(trade.close.format('YYYY-MM-DD'));
    // }
    this.areaChartData = [{ 
      data: dataPoints, 
      label: this.title,
      pointRadius: 1,
      pointBorderWidth: 1,
      fill: 'origin',      
      showLine: true,
      lineTension: 0,
    }];

    // this.areaChartLabels = Array.from(labels);
  }

  @Input() title: string;

  @Input() trades: TradeDto[];

  averageReturn: number;
}
