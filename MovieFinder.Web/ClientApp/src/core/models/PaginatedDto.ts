import {PaginationMetaDto} from "./paginationMetaDto";

export class PageinatedDto<T>{
    data: T;
    paginationMeta: PaginationMetaDto

    constructor(data: T, paginationMeta : PaginationMetaDto) {
        this.data = data;
        this.paginationMeta = paginationMeta;
    }
}