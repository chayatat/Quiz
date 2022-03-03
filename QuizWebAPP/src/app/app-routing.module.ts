import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LandingComponent } from './components/landing/landing.component';
import { RegisterComponent } from './components/register/register.component';
import { ContinueComponent } from './components/continue/continue.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { SummaryComponent } from './components/summary/summary.component';

const routes: Routes = [
  { path : '', pathMatch: 'full', redirectTo: 'landing' },
  { path : 'landing', component: LandingComponent },
  { path : 'register', component: RegisterComponent },
  { path : 'continue', component: ContinueComponent },
  { path : 'quiz', component: QuizComponent },
  { path : 'summary', component: SummaryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
