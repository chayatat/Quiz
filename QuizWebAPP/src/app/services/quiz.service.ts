import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

export class UserGroup {
  userGroupId!: Number;
  userGroupName!: String;
}

export class User {
  userId!: Number;
  userName!: String;
  userGroupId!: Number;
  userStatus!: String;
  userTotalScore!: Number;
}

export class Question {
  questionId!: Number;
  questionName!: String;
  questionSort!: Number;
  TblChoices!: any[];
}

export class UserScore {
  userScoreId!: Number;
  userId!: Number;
  userName!: String;
  userGroupId!: Number;
  userGroupName!: String;
  questionId!: Number;
  choiceId!: Number;
  choiceScore!: Number;
}


@Injectable({
  providedIn: 'root'
})
export class QuizService {

  quizWebAPI: String = "https://localhost:44301/api"
  httpHeaders = new HttpHeaders().set("Content-Type", "application/json");

  constructor(private httpClient:HttpClient) { }

  getUserGroup(){
    return this.httpClient.get(`${this.quizWebAPI}/usergroup`)
      .pipe(
        catchError(this.handleError)
      );
  }

  addRegister(data: User): Observable<any>{
    let api = `${this.quizWebAPI}/register`;
    return this.httpClient.post(api, data)
      .pipe(
        catchError(this.handleError)
      )
  }

  getQuiz(id: any){
    let api = `${this.quizWebAPI}/quiz/${id}`;
    return this.httpClient.get(api, { headers: this.httpHeaders } )
      .pipe(
        catchError(this.handleError)
      );
  }

  getLoad(id: any){
    let api = `${this.quizWebAPI}/load/${id}`;
    return this.httpClient.get(api, { headers: this.httpHeaders } )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveQuiz(data : any []): Observable<any>{
    let api = `${this.quizWebAPI}/save`;
    return this.httpClient.post(api, data)
      .pipe(
        catchError(this.handleError)
      )
  }

  submitQuiz(data : any []): Observable<any>{
    let api = `${this.quizWebAPI}/submit`;
    return this.httpClient.post(api, data)
      .pipe(
        catchError(this.handleError)
      )
  }

  getSummary(id: any){
    let api = `${this.quizWebAPI}/summary/${id}`;
    return this.httpClient.get(api, { headers: this.httpHeaders } )
      .pipe(map((res: any) => {
        return res || {}
      }),
      catchError(this.handleError)
      );
  }

  getContinue(name: any){
    let api = `${this.quizWebAPI}/continue/${name}`;
    return this.httpClient.get(api, { headers: this.httpHeaders } )
      .pipe(map((res: any) => {
        return res || {}
      }),
      catchError(this.handleError)
      );
  }

  handleError(err: HttpErrorResponse){
    let errMsg = '';
    if(err.error instanceof ErrorEvent){
      errMsg = err.error.message;
    }else{
      errMsg = `Error Code: ${err.status}\nMessage: ${err.message}`;
    }
    //console.log(errMsg);
    return throwError(errMsg);
  }
}
