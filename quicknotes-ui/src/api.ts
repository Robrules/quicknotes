import axios from 'axios';

export interface Note {
  id: number;
  text: string;
  created: string;
}

export const getNotes  = () => axios.get<Note[]>('/api/notes');
export const addNote   = (text: string) => axios.post('/api/notes', text);
export const delNote   = (id: number) => axios.delete(`/api/notes/${id}`);


