import type { ConversationHeader } from '@/types/ConversationHeader'
import type { Message } from '@/types/Message'

export default class ConversationApiService {
  apiBaseUrl: string

  constructor() {
    this.apiBaseUrl = 'https://localhost:7192/conversation'
  }

  public async GetConversationHeader(id: number): Promise<ConversationHeader> {
    try {
      const url = `${this.apiBaseUrl}/${id}/header`
      const response = await fetch(url, {
        headers: { 'Content-Type': 'application/json' },
      })
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      const header: ConversationHeader = await response.json()
      return header
    } catch (error) {
      console.error('Error fetching conversation title:', error)
      throw error
    }
  }

  public async CreateNewConversation(): Promise<ConversationHeader> {
    try {
      const url = `${this.apiBaseUrl}/`
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ title: '' }),
      })
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      const createdConversation: ConversationHeader = await response.json()
      console.log('Created conversation with ID:', createdConversation.id)
      return createdConversation
    } catch (error) {
      console.error('Error creating conversation:', error)
      return undefined as unknown as ConversationHeader
    }
  }

  public async SendMessage(id: number, message: Message): Promise<Message> {
    try {
      const url = `${this.apiBaseUrl}/ask/${id}`
      console.trace('messages being sent to server:' + JSON.stringify(message))
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(message),
      })
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      const returnedMessage: Message = await response.json()
      console.log('Received message from server:', returnedMessage)
      return returnedMessage as unknown as Message
    } catch (error) {
      console.error('Error sending message:', error)
      throw error
    }
  }

  public async GetConversationList(): Promise<Array<ConversationHeader>> {
    try {
      const url = `${this.apiBaseUrl}/conversation`
      const response = await fetch(url, {
        headers: { 'Content-Type': 'application/json' },
      })
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      const conversations: Array<ConversationHeader> = await response.json()
      return conversations
    } catch (error) {
      console.error('Error fetching conversation list:', error)
      return []
    }
  }

  public async DeleteConversation(id: number): Promise<boolean> {
    try {
      const url = `${this.apiBaseUrl}/${id}`
      const response = await fetch(url, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
      })
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }
      return true
    } catch (error) {
      console.error('Error deleting conversation:', error)
      return false
    }
  }
}
