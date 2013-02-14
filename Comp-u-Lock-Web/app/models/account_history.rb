class AccountHistory < ActiveRecord::Base
  attr_accessible :account_id, :domain, :title, :last_visited, :url, :visit_count

  validates :account_id, presence: true
  validates :domain, presence: true
  validates :last_visited, presence: true
  
  belongs_to :account
end
