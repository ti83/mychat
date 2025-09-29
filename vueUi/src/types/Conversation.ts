import type { Message } from './Message'

export interface Conversation {
  id: number
  title: string
  messages: Message[]
}
