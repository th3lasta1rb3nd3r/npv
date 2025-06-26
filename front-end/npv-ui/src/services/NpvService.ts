import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { INpvResult } from '../interfaces/INpvResult';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class NpvService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    calculateNPV(cashFlows: number[], lowerRate: number, upperRate: number, increment: number): Observable<INpvResult[]> {
        return this.http.post<INpvResult[]>(`${this.apiUrl}/api/Npv/calculateNPV`, {
            cashFlows,
            lowerBoundRate: lowerRate,
            upperBoundRate: upperRate,
            increment: increment
        });
    }
}
