import { Component, Inject, Input, Sanitizer } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Binary } from '@angular/compiler';
import { forEach } from '@angular/router/src/utils/collection';
import { bypassSanitizationTrustHtml } from '@angular/core/src/sanitization/bypass';


@Component({
  selector: 'app-thumbnail',
  templateUrl: './thumbnail.component.html',
})
export class ThumbnailComponent {
  public valid: boolean;
  public images: Array<Array<string>>;
  public executing: boolean;
  public executingimages: boolean;
  public executedimages: boolean;
  public baseUrl: string;
  private http: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.valid = false;
    this.executing = false;
    this.executingimages = false;
    this.http = http;
    this.baseUrl = baseUrl;
  }

  checkvalid(directory: string) {
    this.executing = true;

    if (directory !== '') {

      this.http.get<boolean>(this.baseUrl + 'api/SampleData/AddImages',
        { params: new HttpParams().set('directory', directory) }).subscribe(result => {
          this.valid = result;
          this.executing = false;
      }, error => console.error(error));

    } else {
      this.valid = false;
      this.executing = false;
    }
  }

  GetImages() {
    this.executingimages = true;

    this.http.get<Array<Array<string>>>(this.baseUrl + 'api/SampleData/GetImages').subscribe(result => {
      this.images = result;
      this.executingimages = false;
      // this.decodeimages();
      }, error => console.error(error));

  }

  public setvalidfalse() {
    this.valid = false;
  }

  private decodeimages() {
    this.images.forEach(function (value, index) {
      const ImageData = value[1];
      const byte = atob(ImageData);
      this[index] = [value[0], byte];
    }, this.images);
  }
}



