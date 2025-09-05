import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-payment-result',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="card" style="max-width:560px;margin:24px auto;padding:24px;text-align:center">
      <h2 *ngIf="success; else fail">Payment successful</h2>
      <ng-template #fail><h2>Payment failed</h2></ng-template>
      <p *ngIf="success; else tryagain">Thank you! Your payment was processed successfully.</p>
      <ng-template #tryagain><p>There was an issue processing your payment. You can try again or choose cash on delivery.</p></ng-template>
      <a class="btn" routerLink="/store" style="margin-top:12px;display:inline-block">Back to store</a>
    </div>
  `
})
export class PaymentResultComponent {
  private route = inject(ActivatedRoute);
  success = false;
  constructor(){
    this.route.queryParamMap.subscribe(p => {
      this.success = (p.get('success') || '').toLowerCase() === 'true';
    });
  }
}


