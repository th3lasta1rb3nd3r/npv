import { Component, ViewChild } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartType } from 'chart.js';

import { INpvResult } from '../../interfaces/INpvResult';

@Component({
  selector: 'app-chart',
  imports: [BaseChartDirective],
  templateUrl: './chart.component.html'
})
export class ChartComponent {

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  public chartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'NPV'
      }
    ]
  };

  updateChart(results: INpvResult[]) {
    this.chartData.labels = results.map(r => r.discountRate.toString());
    this.chartData.datasets[0].data = results.map(r => r.npvValue);
    this.chart?.update();
  }
}
