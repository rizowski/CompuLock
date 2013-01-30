CompULockWeb::Application.routes.draw do
  get "home/index"

  root :to => "home#index"
  devise_for :users
  devise_scope :user do 
    match '/users/sign_out' => 'devise/sessions#destroy', :as => :destroy_user_session
  end

  match ':controller(/:action(/:id))(.:format)'
end
