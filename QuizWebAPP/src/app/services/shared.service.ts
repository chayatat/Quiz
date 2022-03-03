import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  userShared: any;
  userScoresShared: any[] = [];

  constructor() { }

  setUserShared(data:any){
    this.userShared = data;
  }

  getUserShared(){
     return this.userShared;
  }

  setUserScoresShared(data:any[]){
    this.userScoresShared = data;
  }

  getUserScoresShared(){
    return this.userScoresShared;
  }
}
