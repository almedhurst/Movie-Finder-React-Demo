export const truncate = (input: string, charLimit: number) => 
    input !== null && input.length > charLimit 
        ? `${input.substring(0, charLimit)}...` 
        : input;

export const urlFriendly = (input: string) => 
    input == undefined 
        ? '' 
        : input.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase();