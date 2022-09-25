import { Photo } from "./photo";


export interface Member {
    id: number;
    username: string;
    photoUrl: string;
    age: number;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    knownAs: string;
    city: string;
    country: string;
    photos: Photo[];
    dateOfBirth:Date
}



