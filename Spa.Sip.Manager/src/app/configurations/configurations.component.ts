import { Component, OnInit } from '@angular/core';
import { PhoneConfigService } from '../core/phone-config.service';
import { ContentModel } from '../model/content-model';

@Component({
  selector: 'app-configurations',
  templateUrl: './configurations.component.html',
  styleUrls: ['./configurations.component.css']
})
export class ConfigurationsComponent implements OnInit {
  configurations: ContentModel[];

  constructor(private _phoneConfigService: PhoneConfigService) { }

  ngOnInit() {
    this._phoneConfigService.getConfigurations().subscribe(configurations => {
      this.configurations = configurations;
    }, error => console.log(error));
  }

}
