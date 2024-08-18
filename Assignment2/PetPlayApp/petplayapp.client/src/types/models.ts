export class LoginDto {
    username?: string;
    password?: string;
}
export class UserDetailsDto {
    userId?: string;
    username?: string;
    email?: string;
    age?: number;
    bio?: string;
    profilePicture?: Uint8Array;
    interest?: UserInterest;
    status?: UserStatus;
}

export class CreateUserDto {
    username?: string;
    email?: string;
    password?: string;
    age?: number;
    bio?: string;
    profilePicture?: Uint8Array;
}

export class UserSearchDto {
    userId?: string;
    username?: string;
}

export class StoryDetailsDto {
    storyId?: string;
    storyCreatorId?: string;
    storyCreatorName?: string;
    storyProfilePicture?: Uint8Array;
    imageData?: Uint8Array;
    dateTimePosted?: Date;
}

export class ResetPasswordDto {
    email?: string;
    oldPassword?: string;
    newPassword?: string;
}

export enum UserInterest {
    Unlisted = 'Unlisted',
    Matched = 'Matched',
    NotMatched = 'NotMatched'
}

export enum UserStatus {
    Unlisted = 'Unlisted',
    Mammals= 'Mammals',
    Reptiles = 'Reptiles',
    Amphibians = 'Amphibians',
    Birds = 'Birds'
}
export class Match {
    user1Id?: string;
    user2Id?: string;
    matchStatus?: number;
    user1Response?: number;
    user2Response?: number;
}