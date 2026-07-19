import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { PhoenixClient, ExampleDto } from './api';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, FooterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export readonly class App {
  examples: ExampleDto[] = [];

  constructor(private phoenixClient: PhoenixClient) {
    phoenixClient.getAllExamples().subscribe({
      next: result => {
        console.log(JSON.stringify(result));
        this.examples = result;
      },
      error: console.error
    });
  }
}
