<script setup lang="ts">
import MyChat from './components/MyChat.vue'
</script>

<template>
  <header>
    <div class="header-content">
      <img alt="Chat Logo" class="logo" src="./assets/icons8-chat-96.png" width="48" height="48" />
      <div class="title">My Chat</div>
    </div>
  </header>
  <main>
    <MyChat />
  </main>
</template>

<style scoped>
.header-content {
  display: flex;
  align-items: center;
}

.logo {
  margin: 0;
}

.title {
  margin-left: 1rem;
  color: black;
  font-weight: bold;
  font-size: 1.75rem;
}

header {
  width: 100%;
  padding: 1rem;
}

main {
  width: 100%;
  margin: 0 auto;
  padding: 0 1rem;
}
</style>
