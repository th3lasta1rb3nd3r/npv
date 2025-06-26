import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

import { ChartComponent } from './chart/chart.component';

import { NpvService } from '../services/NpvService';
import { INpvResult } from '../interfaces/INpvResult';


@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet, FormsModule, ChartComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'NPV Calculator';

  cashFlowsInput = '';
  lowerRate = 1;
  upperRate = 15;
  increment = 0.25;
  results: INpvResult[] = [];

  @ViewChild(ChartComponent) child!: ChartComponent;

  error = '';

  constructor(private npvService: NpvService) { }

  clearErrors(form: NgForm) {
    this.error = '';
  }

  onSubmit(form: NgForm) {
    this.child.updateChart([]);

    let cashFlows: number[] = [];

    if (this.cashFlowsInput !== '') {
      cashFlows = this.cashFlowsInput.split(',').map(Number);
    }

    this.npvService
      .calculateNPV(cashFlows, this.lowerRate, this.upperRate, this.increment)
      .subscribe({
        next: (data) => {
          this.child.updateChart(data);
        },
        error: (err) => {
          this.error = err.error;
        }
      });
  }
}
