import {Box, Pagination, Typography} from "@mui/material";
import {PaginationMetaDto} from "../core/models/paginationMetaDto";

interface Props {
    paginationData: PaginationMetaDto;
    onPageChange: (page: number) => void;
}

export default function PaginationComponent({paginationData, onPageChange}: Props){
    const {pageIndex, pageSize, count, pageCount} = paginationData;
    return (
        <Box display='flex' justifyContent='space-between' alignItems='center' sx={{mb: 1}}>
            <Typography>
                Displaying {(pageIndex-1)*pageSize+1}-
                {pageIndex*pageSize > count 
                    ? count 
                    : pageIndex*pageSize} of {count} items
            </Typography>
            <Pagination
                color='secondary'
                size='large'
                count={pageCount}
                page={pageIndex}
                onChange={(e, page) => onPageChange(page)}
            />
        </Box>
    )
}