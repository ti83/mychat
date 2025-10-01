import type { ConversationHeader } from '@/types/ConversationHeader'
import type { Conversation } from '@/types/Conversation'
import { defineStore } from 'pinia'
import ConversationApiService from '@/services/ConversationApiService'
const apiService = new ConversationApiService()

class ConvesationStoreState {
  conversations: Array<ConversationHeader> = []
  currentConversation: Conversation | null = null
}

export const ConversationStore = defineStore('ConversationStore', {
  state: () => ({
    conversations: [] as Array<ConversationHeader>,
    currentConversation: null as Conversation | null,
  }),
  actions: {
    setConversations(state: ConvesationStoreState, conversations: Array<ConversationHeader>) {
      state.conversations = conversations
    },
    addConversation(state: ConvesationStoreState, conversation: ConversationHeader) {
      state.conversations.push(conversation)
    },
    setCurrentConversation(state: ConvesationStoreState, conversation: Conversation) {
      state.currentConversation = conversation
    },
    updateTitle(state: ConvesationStoreState, id: number, title: string) {
      const convo = state.conversations.find((c) => c.id === id)
      if (convo) {
        convo.title = title
      }
    },
    async fetchConversations(state: ConvesationStoreState) {
      try {
        // Assuming there's an endpoint to get all conversation headers
        const response = await fetch(`${apiService.apiBaseUrl}/`, {
          headers: { 'Content-Type': 'application/json' },
        })
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`)
        }
        const conversations: Array<ConversationHeader> = await response.json()
        this.setConversations(state, conversations)
      } catch (error) {
        console.error('Error fetching conversations:', error)
      }
    },
    deleteConversation(state: ConvesationStoreState, id: number) {
      state.conversations = state.conversations.filter((c) => c.id !== id)
      if (state.currentConversation && state.currentConversation.id === id) {
        state.currentConversation = null
      }
    },
  },
  getters: {
    getConversations(state: ConvesationStoreState) {
      return state.conversations
    },
    getCurrentConversation(state: ConvesationStoreState) {
      return state.currentConversation
    },
  },
})
