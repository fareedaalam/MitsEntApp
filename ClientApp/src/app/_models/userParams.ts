import { User } from "./user";

export class UserParams {
    gender: string;
    minAge = 10;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';
    knownAs = 'constestant';

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
        this.knownAs =user.knownAs === 'contestant' ? 'member' :'contestant';
    }

}
