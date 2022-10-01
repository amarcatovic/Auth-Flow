import { Weather } from './../../_interfaces/weather.model';
import { RepositoryService } from './../../shared/services/repository.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  public weather: Weather[];

  constructor(private repository: RepositoryService) { }

  ngOnInit(): void {
    this.getWeather();
  }

  public getWeather = () => {
    const apiAddress: string = "test";
    this.repository.getData(apiAddress)
      .subscribe(res => {
        this.weather = res as Weather[];
      })
  }
}
