import { Component, OnInit } from '@angular/core';
import { ParseTradesModel, InvestmentsClient } from 'src/app/firewatch-api';
import { FormBuilder, Validators } from '@angular/forms';

interface SupportedFormat {
  display: string;
  value: string;

}

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.css'],
})
export class ImportComponent implements OnInit {

  debug = true;

  supportedFormats: SupportedFormat[] = [
    { display: "TradeLog", value: "TradeLog" },
  ];

  selectedFormat: SupportedFormat = this.supportedFormats[0];

  constructor(private fb: FormBuilder, private client: InvestmentsClient) { } // , private client: InvestmentsClient

  importForm = this.fb.group({
    fileContents: [ null, [ Validators.required ] ],
    fileSource: [ null, Validators.required ],
    file: [ '' ],
    format: [ this.supportedFormats[0].value ],
  });

  ngOnInit(): void {
  }

  onFileChange(event: Event) {
    if ((event.target as HTMLInputElement).files && (event.target as HTMLInputElement).files.length) {
      const file = (event.target as HTMLInputElement).files[0];
      console.log(`Reading ${file.name}...`, );
      this.importForm.patchValue({
        fileSource: file
      });

      const fileReader = new FileReader();
      fileReader.onload = () => {
        this.importForm.patchValue({
          fileContents: fileReader.result
        });
      }

      fileReader.readAsText(file);
    } else {
      console.warn('No file selected.');
    }    
  }

  onFormatChange(event: Event) {
    const format = (event.target as HTMLInputElement).value;
    console.log('Updating format to ' + format);
    this.importForm.patchValue({ 
      format: format 
    });
  }

  onSubmit() {
    console.log('Form value...', this.importForm.value);

    const model = new ParseTradesModel({
      content: this.importForm.get('fileContents').value,
      format: this.importForm.get('format').value
    });

    console.log('Submitting...', model);

    this.client.importTrades(model)
      .subscribe(resp => {
      console.log(`Added ${resp.createdIds.length} trades (${resp.duplicates} ignored as duplicates).`);
    });
    
    this.importForm.get('fileContents').value
  }

}
