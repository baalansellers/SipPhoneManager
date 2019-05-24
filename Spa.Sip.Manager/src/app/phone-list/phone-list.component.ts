import { Component, OnInit } from '@angular/core';
import { ContentModel } from '../model/content-model';
import { PhoneConfigService } from '../core/phone-config.service';

@Component({
  selector: 'app-phone-list',
  templateUrl: './phone-list.component.html',
  styleUrls: ['./phone-list.component.css']
})
export class PhoneListComponent implements OnInit {
  phones: ContentModel[];

  constructor(
    private _phoneConfigService: PhoneConfigService
  ) { }

  ngOnInit() {
    this._phoneConfigService.getConfigurations().subscribe(phones => {
      this.phones = phones;
    }, error => {console.log(error)})
  }

  getMacAddress(name: string) {
    return name.replace("SEP", "").replace(".cnf.xml", "");
  }
}
