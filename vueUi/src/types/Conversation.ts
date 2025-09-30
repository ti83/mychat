import type { Message } from './Message'
import type { ConversationHeader } from './ConversationHeader'

export interface Conversation extends ConversationHeader {
  messages: Message[]
}
