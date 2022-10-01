import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherComponent } from './weather/weather.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [WeatherComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'weather', component: WeatherComponent }
    ])
  ]
})
export class WeatherModule { }
