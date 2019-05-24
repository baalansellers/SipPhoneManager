import { Component, OnInit, Input } from '@angular/core';
import { PhoneConfigService } from '../core/phone-config.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-config-content',
  templateUrl: './view-config-content.component.html',
  styleUrls: ['./view-config-content.component.css']
})
export class ViewConfigContentComponent implements OnInit {
  content: string;

  constructor(
    private _phoneConfigService: PhoneConfigService,
    private _route: ActivatedRoute,
    private _router: Router
  ) { }

  ngOnInit() {
    const name = this._route.snapshot.paramMap.get('name');

    this._phoneConfigService.getConfiguration(name).subscribe(content => {
      console.log(content);
      this.content = content;
    }, error => {console.log(error)})
  }
}
