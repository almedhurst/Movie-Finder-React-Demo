import {useState} from "react";
import {Divider, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, Typography} from "@mui/material";

interface Props {
    items: { value: string, text: string }[];
    selectedItem?: string;
    onChange: (selection: string) => void;
    label: string;
    fullWidth: boolean;
}

export default function SelectList({items, selectedItem, onChange, label, fullWidth}: Props) {
    const [selected, setSelected] = useState(selectedItem || items[0].value);

    const handleSelected = (event: SelectChangeEvent) => {
        setSelected(event.target.value);
        onChange(event.target.value);
    }

    return (
        <>
            {
                fullWidth ? (
                    <>
                        <Typography variant='h6'>{label}</Typography>
                        <Divider/>
                        <FormControl size='small' sx={{width: '100%'}}>
                            <Select value={selected}
                                    onChange={handleSelected}
                            >
                                {items.map(item => (
                                    <MenuItem value={item.value} key={item.value}>{item.text}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </>
                ) : (
                    <>
                        <FormControl size='small' sx={{mb: 1}}>
                            <InputLabel id='select-list-label'>{label}</InputLabel>
                            <Select value={selected}
                                    onChange={handleSelected}
                                    labelId='select-list-label'
                                    label={label}
                            >
                                {items.map(item => (
                                    <MenuItem value={item.value} key={item.value}>{item.text}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </>
                )
            }
        </>
    )
}