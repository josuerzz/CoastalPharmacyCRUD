import { Component, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faLinkedin, faGithub } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-footer',
  imports: [FontAwesomeModule],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer 
{
  currentYear = signal(new Date().getFullYear());
  appVersion = signal('1.0.0');
  faLinkedin = faLinkedin;
  faGithub = faGithub;
}
