import {Box, Button, Checkbox, Chip, Divider, FormControlLabel, FormGroup, Typography} from "@mui/material";
import {useState} from "react";

interface Props {
    items: { value: string, text: string }[];
    selectedItems?: string[];
    onChange: (selection: string[]) => void;
    label: string;
}

export default function CheckboxSelectList({items, selectedItems, onChange, label}: Props) {

    const [checkedItems, setCheckedItems] = useState(selectedItems || []);

    const handleChecked = (value: string) => {
        const currentIndex = checkedItems.findIndex(item => item === value);
        let newChecked: string[] = [];
        if (currentIndex === -1) newChecked = [...checkedItems, value];
        else newChecked = checkedItems.filter(item => item !== value);
        setCheckedItems(newChecked);
        onChange(newChecked);
    }

    return (
        <>
            <Typography variant='h6'>{label}</Typography>
            <Divider/>
            <Box sx={{maxHeight: 300, overflowY: 'scroll', mb: 1}}>
                <FormGroup>
                    {items.map(item => (
                        <FormControlLabel
                            control={<Checkbox
                                checked={checkedItems.indexOf(item.value) !== -1}
                                onClick={() => handleChecked(item.value)}
                            />}
                            label={item.text}
                            key={item.value}/>
                    ))}
                </FormGroup>
            </Box>
            <Box sx={{ display: checkedItems?.length > 0 ? 'block' : 'none'}}>
                <Box sx={{display: 'flex', flexDirection: 'row', alignItems: 'center'}}>
                    <Typography>Selected:</Typography>
                    <Button type='button' size='small' sx={{ml: 'auto'}}
                            onClick={() => {setCheckedItems([]); onChange([]);}}
                    >Clear all</Button>
                </Box>

                {checkedItems?.map(item => (
                    <Chip size='small' sx={{mb: 1, mr: 1}} onDelete={() => handleChecked(item)}
                          label={items.find(x => x.value === item)?.text} key={item} />
                ))}
            </Box>
            
        </>
    )
}